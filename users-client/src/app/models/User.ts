import { UserRole } from "../enum/userRole";
import Message from "./Message";

export default class User {
    id: number = -1;
    firstName: string = "";
    lastName: string = "";
    userName: string = "";
    password: string = "";
    email: string = "";
    photoUrl: string = "";
    message: string = "";
    statusCode: string = "";
    user !: User;
    role: UserRole = UserRole.USER;
    messages: Array<Message> = [];
}