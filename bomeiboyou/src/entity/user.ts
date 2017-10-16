export class User {
    //0为购买用户,1为订购用户
    constructor(public id: string, public password: string, public confirmPassword: string, public userType: number) {

    }
}