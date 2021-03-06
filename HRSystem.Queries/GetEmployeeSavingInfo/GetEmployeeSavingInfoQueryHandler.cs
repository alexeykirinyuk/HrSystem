﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Web.Dtos;
using MediatR;

namespace HRSystem.Queries.GetEmployeeSavingInfo
{
    public class
        GetEmployeeSavingInfoQueryHandler : IRequestHandler<GetEmployeeSavingInfoQuery, GetEmployeeSavingInfoQueryResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeInfoService _attributeInfoService;

        public GetEmployeeSavingInfoQueryHandler(IEmployeeService employeeService, IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);

            _employeeService = employeeService;
            _attributeInfoService = attributeInfoService;
        }

        public async Task<GetEmployeeSavingInfoQueryResponse> Handle(GetEmployeeSavingInfoQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await GetEmployee(request.IsCreate, request.Login);
            var attributes = await _attributeInfoService.GetAll().ConfigureAwait(false);
            var employees = await _employeeService.GetAll().ConfigureAwait(false);

            return new GetEmployeeSavingInfoQueryResponse
            {
                Employee = Mapper.Map<EmployeeDto>(employee),
                Attributes =  Mapper.Map<ICollection<AttributeInfoDto>>(attributes.ToArray()),
                Employees =  Mapper.Map<ICollection<EmployeeDto>>(employees.ToArray())
            };
        }

        private Task<Employee> GetEmployee(bool isCreate, string login)
        {
            if (!isCreate)
            {
                return _employeeService.GetByLogin(login);
            }

            return Task.FromResult(new Employee
            {
                Login = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                ManagerLogin = null,
                Manager = null,
                Attributes = new List<AttributeBase>(),
                JobTitle = string.Empty,
                Office = string.Empty,
                Phone = string.Empty
            });
        }
    }
}