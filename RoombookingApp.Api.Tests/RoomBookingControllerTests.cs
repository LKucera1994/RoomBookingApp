﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using RombookingApp.Api.Controllers;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoombookingApp.Api.Tests
{
    public class RoomBookingControllerTests
    {
        private Mock<IRoomBookingRequestProcessor> _roomBookingProcessor;
        private RoomBookingController _controller;
        private RoomBookingRequest _request;
        private RoomBookingResult _result;



        public RoomBookingControllerTests()
        {
            _roomBookingProcessor =  new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_roomBookingProcessor.Object);

            _result = new RoomBookingResult();
            _request = new RoomBookingRequest();

            _roomBookingProcessor.Setup(x => x.BookRoom(_request)).Returns(_result);

        }


        [Theory]
        [InlineData(1,true, typeof(OkObjectResult), BookingResultFlag.Success)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]


        public async Task ShouldCallBookingMethodWhenValid(int expectedMethodCalls, bool isModelValid,
            Type expectedActionResultType, BookingResultFlag bookingResultFlag)
        {
            // Arrange
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "ErrorMessage");

            }
            _result.Flag = bookingResultFlag;

            //Act
            var result = await _controller.BookRoom(_request);

            //Assert 
            result.ShouldBeOfType(expectedActionResultType);
            _roomBookingProcessor.Verify(x=> x.BookRoom(_request), Times.Exactly(expectedMethodCalls));
        }


    }
}
