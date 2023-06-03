import User from "./User";

export default class Message {
    id: number = -1;
    content: string = "";
    userId: number = -1;
    user!: User;
}