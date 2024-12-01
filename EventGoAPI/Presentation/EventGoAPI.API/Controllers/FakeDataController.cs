using Azure.Identity;
using EventGoAPI.Application.Features.Command.Event.CreateEvent;
using EventGoAPI.Application.Features.Command.FakeData.CreateFakeEvent;
using EventGoAPI.Application.Features.Command.FakeData.CreateFakeMessage;
using EventGoAPI.Application.Features.Command.FakeData.CreateFakeParticipant;
using EventGoAPI.Application.Features.Command.User.CreateUser;
using EventGoAPI.Domain.Entities;
using EventGoAPI.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EventGoAPI.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FakeDataController : Controller
    {
        private IMediator _mediator;
        public FakeDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFakeDataAsync()
        {
            var faker = new Bogus.Faker("tr");

            double minLatitude = 36.0, maxLatitude = 42.0;
            double minLongitude = 26.0, maxLongitude = 45.0;

            List<User> fakeUsers = new List<User>();
            List<Event> fakeEvents = new List<Event>();
            List<Participant> fakeParticipants = new List<Participant>();
            List<Message> fakeMessages = new List<Message>();

            for (int i = 0; i < 1000; i++)
            {
                double latitude = faker.Random.Double(minLatitude, maxLatitude);
                double longitude = faker.Random.Double(minLongitude, maxLongitude);

                string city = faker.Address.City();
                string address = faker.Address.StreetAddress();
                string country = "Türkiye";
                var username = faker.Internet.UserName();

                var request = new CreateUserCommandRequest
                {
                    Username = username,
                    PasswordHash = username,
                    Email = faker.Internet.Email(),
                    Address = $"{address}, {city}, {country}",
                    City = city,
                    Country = country,
                    Latitude = latitude,
                    Longitude = longitude,
                    Interests = faker.Make(3, () => faker.PickRandom<EventCategory>()).ToList(),
                    Name = faker.Name.FirstName(),
                    Surname = faker.Name.LastName(),
                    BirthDate = faker.Date.Past(50, DateTime.Now.AddYears(-18)),
                    Gender = faker.Random.Bool(),
                    PhoneNumber = faker.Phone.PhoneNumber(),
                    Image = null,
                };
                CreateUserCommandResponse response = await _mediator.Send(request);
                fakeUsers.Add(response.User);
            }

            for (int i = 0; i < 10000; i++)
            {
                double latitude = faker.Random.Double(minLatitude, maxLatitude);
                double longitude = faker.Random.Double(minLongitude, maxLongitude);

                string city = faker.Address.City();
                string address = faker.Address.StreetAddress();
                string country = "Türkiye";

                var randomUser = faker.PickRandom(fakeUsers);

                var request = new CreateFakeEventCommandRequest
                {
                    CreatedById = randomUser.Id,
                    Name = faker.Lorem.Sentence(3),
                    Description = faker.Lorem.Paragraph(),
                    Date = faker.Date.Future(),
                    Duration = faker.Random.Int(60, 500),
                    Address = $"{address}, {city}, {country}",
                    City = city,
                    Country = country,
                    Latitude = latitude,
                    Longitude = longitude,
                    Category = faker.PickRandom<EventCategory>(),
                    Image = null,
                    IsApproved = faker.Random.Bool(),
                };

                CreateFakeEventCommandResponse response = await _mediator.Send(request);
                fakeEvents.Add(response.Event);
            }

            for (int i = 0; i < 15000; i++)
            {
                var randomUser = faker.PickRandom(fakeUsers);
                var randomEvent = faker.PickRandom(fakeEvents);

                var request = new CreateFakeParticipantCommandRequest
                {
                    UserId = randomUser.Id,
                    EventId = randomEvent.Id,
                };

                CreateFakeParticipantCommandResponse response = await _mediator.Send(request);
                if (response.Success)
                {
                    var participant = response.Participant;
                    fakeParticipants.Add(participant);

                    for (int j = 0; j < 1500; j++)
                    {
                        var messageDate = faker.Date.Between(randomEvent.Date, DateTime.Now);
                        var request1 = new CreateFakeMessageCommandRequest
                        {
                            SenderId = randomUser.Id,
                            SendingTime = messageDate,
                            Message = faker.Lorem.Sentence(),
                            EventId = randomEvent.Id,
                        };

                        CreateFakeMessageCommandResponse response1 = await _mediator.Send(request1);
                        fakeMessages.Add(response1.Message);
                    }
                }
                else
                {
                    i--;
                }
            }

            return Ok(new { Users = fakeUsers, Events = fakeEvents, Participants = fakeParticipants, Messages = fakeMessages });
        }
    }
}
