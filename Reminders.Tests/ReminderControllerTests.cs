using Microsoft.AspNetCore.Mvc;
using Moq;
using RemindersDTI.Controllers;
using RemindersDTI.DTOs;
using RemindersDTI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Reminders.Tests
{
    public class RemindersControllerTests
    {
        [Fact]
        public async Task CreateReminder_DataNoPassado_RetornaBadRequest()
        {
            // 1. arrange com dados inválidos (data no passado) falsos
            var mockService = new Mock<IReminderService>();
            var controller = new RemindersController(mockService.Object);

            var dto = new CreateReminderDto
            {
                Name = "Lembrete Antigo",
                ReminderDate = DateTime.Now.AddDays(-1) // Força a data para ontem
            };

            // 2. serve para simular a validação do modelo, que normalmente é feita pelo framework
            var result = await controller.CreateReminder(dto);

            // 3. faz a asserção para verificar se o resultado é um BadRequestObjectResult
            Assert.IsType<BadRequestObjectResult>(result); // 400 Bad Request
        }

        [Fact]
        public async Task CreateReminder_DataNoFuturo_RetornaCreated()
        {
            // 1. faz a simulação do serviço para retornar um lembrete válido 
            var mockService = new Mock<IReminderService>();

            mockService.Setup(s => s.CreateAsync(It.IsAny<CreateReminderDto>()))
                       .ReturnsAsync(new ReminderDto { Id = Guid.NewGuid(), Name = "Lembrete Futuro" });

            var controller = new RemindersController(mockService.Object);

            var dto = new CreateReminderDto
            {
                Name = "Lembrete Futuro",
                ReminderDate = DateTime.Now.AddDays(1) // Força a data para amanhã
            };

            // 2 faz a chamada ao método do controller
            var result = await controller.CreateReminder(dto);

            // 3. faz a asserção para verificar se o resultado é um CreatedResult
            Assert.IsType<CreatedResult>(result); // 201 Created
        }

        [Fact]
        public async Task GetAll_RetornaOk_ComListaDeLembretes()
        {
            // 1. arrange para simular o serviço e retornar uma lista de lembretes
            var mockService = new Mock<IReminderService>();

            mockService.Setup(s => s.GetAllAsync())
                       .ReturnsAsync(new List<ReminderDto> { new ReminderDto(), new ReminderDto() });

            var controller = new RemindersController(mockService.Object);

            // 2. faz a chamada ao método do controller
            var result = await controller.GetAllReminders();

            // 3. faz a asserção para verificar se o resultado é um OkObjectResult e se contém a lista de lembretes
            var okResult = Assert.IsType<OkObjectResult>(result);
            var lista = Assert.IsAssignableFrom<IEnumerable<ReminderDto>>(okResult.Value);
            Assert.Equal(2, lista.Count()); // Verifica se vieram os 2 itens simulados
        }

        [Fact]
        public async Task Delete_IdExistente_RetornaNoContent()
        {
            // 1. arrange para simular o serviço e retornar verdadeiro quando o lembrete for deletado
            var mockService = new Mock<IReminderService>();
            mockService.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new RemindersController(mockService.Object);

            // 2. faz a chamada ao método do controller
            var result = await controller.DeleteReminder(Guid.NewGuid());

            // 3. faz a asserção para verificar se o resultado é um NoContentResult
            Assert.IsType<NoContentResult>(result); // 204 No Content
        }

        [Fact]
        public async Task Delete_IdInexistente_RetornaNotFound()
        {
            // 1. arrange para simular o serviço e retornar falso quando o lembrete não for encontrado
            var mockService = new Mock<IReminderService>();
            mockService.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new RemindersController(mockService.Object);

            // 2. faz a chamada ao método do controller
            var result = await controller.DeleteReminder(Guid.NewGuid());

            // 3. faz a asserção para verificar se o resultado é um NotFoundResult
            Assert.IsType<NotFoundResult>(result); // 404 Not Found
        }
    }
}