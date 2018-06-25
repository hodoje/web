export class RegistrationModel{
  username: string;
  password: string;
  name: string;
  lastname: string;
  email: string;
  gender: string;
  nationalIdentificationNumber: string;
  phoneNumber: string;

  constructor(
    username: string, 
    password: string, 
    name: string, 
    lastname: string, 
    email: string, 
    gender: string,
    nationalIdentificationNumber: string,
    phoneNumber: string){
    this.username = username;
    this.password = password;
    this.name = name;
    this.lastname = lastname;
    this.email = email;
    this.gender = gender;
    this.nationalIdentificationNumber = nationalIdentificationNumber;
    this.phoneNumber = phoneNumber;
  }
}