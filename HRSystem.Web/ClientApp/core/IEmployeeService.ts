import {Employee} from "../models/Employee";
import {AttributeInfo} from "../models/AttributeInfo";
import {AttributeType} from "../models/AttributeType";
import {StringHelper} from "../helpers/StringHelper";


export class GetAllEmployeesResponse {
    public employees: Array<Employee>;
    public attributes: Array<AttributeInfo>;
    public managerNames: Array<string>;
    public offices: Array<string>;
    public jobTitles: Array<string>;

    public constructor(response?: GetAllEmployeesResponse) {
        if (response == null) {
            return;
        }

        this.employees = response.employees.map(e => new Employee(e));
        this.attributes = response.attributes.map(a => new AttributeInfo(a));
        this.managerNames = response.managerNames;
        this.offices = response.offices;
        this.jobTitles = response.jobTitles;
    }
}

export class GetAllAttributesResponse {
    attributes: Array<AttributeInfo>;

    public constructor(response?: GetAllAttributesResponse) {
        if (response == null) {
            return;
        }

        this.attributes = response.attributes.map(a => new AttributeInfo(a));
    }
}

export class EmployeeSavingInfoResponse {
    employee: Employee;
    employees: Array<Employee>;
    attributes: Array<AttributeInfo>;

    public constructor(response?: EmployeeSavingInfoResponse) {
        if (response == null) {
            return;
        }

        this.employee = new Employee(response.employee);
        this.employees = response.employees.map(e => new Employee(e));
        this.attributes = response.attributes.map(a => new AttributeInfo(a));
    }
}

export interface ISaveEmployeeParams {
    login: string;
    firstName: string;
    lastName: string;
    email: string;
    jobTitle: string;
    office: string;
    phone: string;
    managerLogin: string;
    attributes: IEmployeeAttribute[];
    isCreateCommand: boolean;
}

export class AddEmployeeParams implements ISaveEmployeeParams {
    login: string;
    firstName: string;
    lastName: string;
    email: string;
    jobTitle: string;
    office: string;
    phone: string;
    managerLogin: string;
    attributes: IEmployeeAttribute[];
    isCreateCommand: boolean;

    public constructor(params: ISaveEmployeeParams) {
        this.login = params.login;
        this.firstName = params.firstName;
        this.lastName = params.lastName;
        this.isCreateCommand = params.isCreateCommand;

        if (!StringHelper.isNullOrEmpty(params.email)) {
            this.email = params.email;
        }
        if (!StringHelper.isNullOrEmpty(params.jobTitle)) {
            this.jobTitle = params.jobTitle;
        }
        if (!StringHelper.isNullOrEmpty(params.office)) {
            this.office = params.office;
        }
        if (!StringHelper.isNullOrEmpty(params.phone)) {
            this.phone = params.phone;
        }
        if (!StringHelper.isNullOrEmpty(params.managerLogin)) {
            this.managerLogin = params.managerLogin;
        }
        this.attributes = params.attributes
            .filter(a => a.type != AttributeType.Document)
            .filter(a => !StringHelper.isNullOrEmpty(a.value));
    }
}

export interface IEmployeeAttribute {
    attributeInfoId: number;
    type: AttributeType;
    value: string;
}

export interface IEmployeeService {
    getAll(
        manager: string,
        office: string,
        jobTitle: string,
        allAttributes: string,
        attributes: Map<number, string>): Promise<GetAllEmployeesResponse>;

    getEmployeeSavingInfo(login: string, isCreate: boolean): Promise<EmployeeSavingInfoResponse>;

    save(request: ISaveEmployeeParams): Promise<void>;

    deleteDocument(employeeLogin: string, attributeInfoId: number): Promise<void>;
}