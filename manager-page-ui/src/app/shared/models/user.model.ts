export interface UserModel {
    id: string;
    email: string;
    firstName: string;
    userName: string;   
    roles: string[];
    permissions: any;
    code:string;
}