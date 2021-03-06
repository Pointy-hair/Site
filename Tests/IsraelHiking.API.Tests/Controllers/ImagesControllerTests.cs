﻿using IsraelHiking.API.Controllers;
using IsraelHiking.API.Services;
using IsraelHiking.Common;
using IsraelHiking.DataAccessInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IsraelHiking.API.Tests.Controllers
{
    [TestClass]
    public class ImagesControllerTests
    {
        private ImagesController _controller;
        private IRepository _repository;
        private IImageCreationService _imageCreationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = Substitute.For<IRepository>();
            _imageCreationService = Substitute.For<IImageCreationService>();
            var options = Substitute.For<IOptions<ConfigurationData>>();
            options.Value.Returns(new ConfigurationData());
            _controller = new ImagesController(_repository, _imageCreationService, options);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _controller.Dispose();
        }

        [TestMethod]
        public void GetImage_NoUrl_ShouldNotFound()
        {
            var results = _controller.GetImage("42").Result as NotFoundResult;

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetImage_UrlInDatabase_ShouldCreateIt()
        {
            var siteUrl = new ShareUrl
            {
                Id = "1",
                DataContainer = new DataContainer()
            };
            _repository.GetUrlById(siteUrl.Id).Returns(siteUrl);

            var results = _controller.GetImage(siteUrl.Id).Result as FileContentResult;

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetColors()
        {
            var results = _controller.GetColors();

            Assert.AreEqual(new ConfigurationData().Colors.Count, results.Count);
        }

        [TestMethod]
        public void PostDataContainer_ShouldCreateImage()
        {
            var dataContainer = new DataContainer();

            _controller.PostDataContainer(dataContainer).Wait();

            _imageCreationService.Received(1).Create(dataContainer);
        }
    }
}
