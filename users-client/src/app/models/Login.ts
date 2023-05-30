import User from "./User";

export default class Login {
    _user!: User;
    token: string = "";
    message: string = "";
    statusCode: string = "";
}