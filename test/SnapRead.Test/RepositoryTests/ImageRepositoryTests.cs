using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SnapRead.Core.Entities;
using SnapRead.Core.Interfaces;
using SnapRead.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SnapRead.UnitTest.RepositoryTests
{
    public class ImageRepositoryTests
    {

        [Fact]
        public void Get_Should_Return_Zero()
        {
            var _options = new DbContextOptionsBuilder<SnapReadContext>()
                             .UseInMemoryDatabase(Guid.NewGuid().ToString())
                             .Options;
            using (var context = new SnapReadContext(_options))
            {
                var _imageRepository = new Repository<Image>(context);
                var result = _imageRepository.ListAllAsync().Result;
                result.ToList().Count.Should().Be(0);
            }
        }

        [Fact]
        public async Task Save_Image_And_Should_Return_All_Count_As_One()
        {
            var _options = new DbContextOptionsBuilder<SnapReadContext>()
                             .UseInMemoryDatabase(Guid.NewGuid().ToString())
                             .Options;
            using (var context = new SnapReadContext(_options))
            {
                var _imageRepository = new Repository<Image>(context);
                var image = new Image("1", @"c:\users\desktop");

                await _imageRepository.CreateAsync(image);
                context.SaveChanges();

            }

            using (var context = new SnapReadContext(_options))
            {
                var _imageRepository = new Repository<Image>(context);
                var result = await _imageRepository.ListAllAsync();
                result.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public async Task Save_Two_Image_And_Should_Return_All_Count_As_Two()
        {

            var _options = new DbContextOptionsBuilder<SnapReadContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;
            using (var context = new SnapReadContext(_options))
            {
                var _imageRepository = new Repository<Image>(context);
                var image1 = new Image("1", @"c:\users\desktop\image1");
                var image2 = new Image("1", @"c:\users\desktop\image2");

                await _imageRepository.CreateAsync(image1);
                await _imageRepository.CreateAsync(image2);
                context.SaveChanges();

            }

            using (var context = new SnapReadContext(_options))
            {
                var _imageRepository = new Repository<Image>(context);
                var result = await _imageRepository.ListAllAsync();
                result.ToList().Count.Should().Be(2);
            }
        }


    }
}
