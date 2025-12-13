import { Guid } from "guid-typescript";
export class Client {
    id?: Guid
    name: string
    abonementExpireDate: string
    sessionsLeft: number
    abonementStatus?: string
    cardNumber?: string

    constructor(id: Guid, name: string, abonementExpireDate: string, sessionsLeft: number, abonementStatus?: string, cardNumber?: string) {
        this.id = id;
        this.name = name;
        this.abonementExpireDate = abonementExpireDate;
        this.sessionsLeft = sessionsLeft;
        this.abonementStatus = abonementStatus;
        this.cardNumber = cardNumber;
    }
}


