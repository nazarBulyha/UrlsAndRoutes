using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UrlsAndRoutes.Tests
{
    [TestClass]
    public class RouteTests
    {
        private HttpContextBase CreateHttpContext(string targetUrl = null,
                                                  string httpMethod = "GET")
        {
            // Создать имитированный запрос
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            // Создать имитированный ответ
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(
                It.IsAny<string>())).Returns<string>(s => s);

            // Создать имитированный контекст, используя запрос и ответ
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            // Вернуть имитированный контекст
            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action,
                                    object routeProperties = null, string httpMethod = "GET")
        {
            // Организация
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Действие - обработка маршрута
            var result
                = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller,
                action, routeProperties));
        }

        private bool TestIncomingRouteResult(RouteData routeResult,
                                             string controller, string action, object propertySet = null)
        {

            Func<object, object, bool> valCompare = (v1, v2) => 
                StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;

            var result = valCompare(routeResult.Values["controller"], controller)
                && valCompare(routeResult.Values["action"], action);

            if (propertySet == null) return result;

            var propInfo = propertySet.GetType().GetProperties();
            if (propInfo.Any(pi => !routeResult.Values.ContainsKey(pi.Name) || 
                !valCompare(routeResult.Values[pi.Name], pi.GetValue(propertySet, null))))
            {
                result = false;
            }
            return result;
        }

        private void TestRouteFail(string url)
        {
            // Организация
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            // Действие - обработка маршрута
            var result = routes.GetRouteData(CreateHttpContext(url));

            // Утверждение
            Assert.IsTrue(result?.Route == null);
        }

        //[TestMethod]
        //public void TestIncomingRoutes()
        //{
        //    // Проверить URL который мы надеемся получить
        //    TestRouteMatch("~/Admin/Index", "Admin", "Index");

        //    // Проверить значения, получаемые из сегментов
        //    TestRouteMatch("~/One/Two", "One", "Two");

        //    // Удостовериться, что слишком много или слишком мало сегментов
        //    // не приводят к совпадению
        //    TestRouteFail("~/Admin/Index/ThirdSegment");
        //    TestRouteFail("~/Admin");
        //}

        [TestMethod]
        public void TestIncomingRoutes()
        {
            TestRouteMatch("~/", "Home", "Index");
            TestRouteMatch("~/Home", "Home", "Index");
            TestRouteMatch("~/Home/Index", "Home", "Index");
            TestRouteMatch("~/Home/CustomVariable", "Home", "CustomVariable");
            TestRouteMatch("~/Home/Index/All", "Home", "Index");
            TestRouteMatch("~/Home/Index/All/Delete", "Home", "Index",
                new { id = "All", catchcall = "Delete" });
            TestRouteMatch("~/Home/Index/All/Delete/Insert", "Home", "Index",
                new { id = "All", catchcall = "Delete/Insert" });

            TestRouteFail("~/Home/About");
            TestRouteFail("~/Admin/Index");
        }
    }
}