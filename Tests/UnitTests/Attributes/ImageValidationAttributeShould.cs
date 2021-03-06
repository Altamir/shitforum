using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using ImageValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using Optional;
using ShitForum.Attributes;
using ShitForum.Mappers;
using UnitTests.Tooling;
using Xunit;

namespace UnitTests.Attributes
{
    public class ImageValidationAttributeShould
    {
        private readonly MockRepository mr = new MockRepository(MockBehavior.Strict);
        private readonly ImageValidationAttribute attr = new ImageValidationAttribute();

        [Fact]
        public void Success()
        {
            var file = mr.Create<IFormFile>();
            var vi = mr.Create<IValidateImage>();
            vi.Setup(a => a.ValidateAsync(It.IsAny<Option<byte[]>>())).ReturnsT((OneOf.OneOf<Pass, SizeExceeded, InvalidImage, BannedImage>)new Pass());

            var sp = mr.Create<IServiceProvider>();

            var um = mr.Create<IUploadMapper>();
            um.Setup(a => a.ExtractData(file.Object)).Returns(Option.None<byte[]>());

            sp.Setup(a => a.GetService(typeof(IValidateImage))).Returns(vi.Object);
            sp.Setup(a => a.GetService(typeof(IUploadMapper))).Returns(um.Object);
            var vc = new ValidationContext(new object(), sp.Object, new Dictionary<object, object>());
            var res = attr.GetValidationResult(file.Object, vc);

            res.Should().BeNull();
            mr.VerifyAll();
        }

        [Fact]
        public void NotSuccess()
        {
            var file = mr.Create<IFormFile>();
            var vi = mr.Create<IValidateImage>();
            vi.Setup(a => a.ValidateAsync(It.IsAny<Option<byte[]>>())).ReturnsT((OneOf.OneOf<Pass, SizeExceeded, InvalidImage, BannedImage>)new SizeExceeded(1024));

            var sp = mr.Create<IServiceProvider>();

            var um = mr.Create<IUploadMapper>();
            um.Setup(a => a.ExtractData(file.Object)).Returns(Option.None<byte[]>());

            sp.Setup(a => a.GetService(typeof(IValidateImage))).Returns(vi.Object);
            sp.Setup(a => a.GetService(typeof(IUploadMapper))).Returns(um.Object);
            var vc = new ValidationContext(new object(), sp.Object, new Dictionary<object, object>());
            var res = attr.GetValidationResult(file.Object, vc);

            res.Should().NotBeNull();
            res.ErrorMessage.Should().Be("Image must not exceed 1024 bytes");
            mr.VerifyAll();
        }
    }
}
