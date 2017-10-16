import { Time } from './time';

export class Flight {
    public constructor(public id: string, public departureAirportId, public arrivalAirportId,
        public departureTime: Time, public arrivalTime: Time) { }
}