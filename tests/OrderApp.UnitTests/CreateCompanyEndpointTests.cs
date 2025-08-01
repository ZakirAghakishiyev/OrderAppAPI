namespace OrderApp.UnitTests;

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderApp.Core.CompanyAggregate;
using OrderApp.SharedKernel.Interfaces;
using OrderApp.Web.Companies;
using OrderApp.Web.Companies.Create;
using Xunit;

public class CreateCompanyEndpointTests
{
    private readonly ICompanyEndpointService _endpointService;
    private readonly Create _create;
    private readonly Mock<IRepository<Company>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public CreateCompanyEndpointTests()
    {
        _repositoryMock = new Mock<IRepository<Company>>();
        _mapperMock = new Mock<IMapper>();
        _endpointService = new CompanyEndpointService(_repositoryMock.Object, _mapperMock.Object);
        _create = new Create(_endpointService);
    }

    [Fact]
    public async Task HandleAsync_ValidCreateRequest_CallsServiceAndReturnsResponse()
    {
        // Arrange
        var request = new CreateCompanyRequest { Name = "Test" };
        var fakeResponse = Result.Success(new CreateCompanyResponse(1, "Test"));

        var endpointServiceMock = new Mock<ICompanyEndpointService>();
        endpointServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<CreateCompanyRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeResponse);

        //Act
        var response = await endpointServiceMock.Object.CreateAsync(request, CancellationToken.None);


        //Assert
        Assert.IsType<Result<CreateCompanyResponse>>(response);
        var sent = response as Result<CreateCompanyResponse>;
        Assert.Equal(1, sent.Value.Id);
        Assert.Equal("Test", sent.Value.Name);
        Assert.Equal(ResultStatus.Ok, response.Status);
    }
}