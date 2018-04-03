import { Injectable } from '@angular/core';
import { HttpClient,  HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { Appointment } from "../models/appointment";
import { catchError, tap} from "rxjs/operators";
import { of } from "rxjs/observable/of";
import {ResponseType} from "@angular/http";

const httpOptions = {
  defaultHeaders: new HttpHeaders({'Accept': 'application/json', 'Content-Type': 'application/json' }),
  iCalHeaders: new HttpHeaders( {'Accept':'text/ical'})
};

@Injectable()
export class AppointmentService {

  private appointmentUrl = "http://localhost:5000/api/1/appointment";

  constructor(
    private http: HttpClient
  ) { }

  public getAppointment(id: number) : Observable<Appointment> {
    return this.http.get<Appointment>(this.appointmentUrl + '/1', { headers: httpOptions.defaultHeaders}).pipe(
      tap(_ => console.debug('fetched appointment id=${id}')),
      catchError(this.handleError<Appointment>('getAppointment id=${id}'))
    );
  }

  public getICalendar(id: number) : Observable<string> {
    return this.http.get(this.appointmentUrl + '/1', { headers: httpOptions.iCalHeaders, responseType: 'text' });
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      console.error(error);
      console.debug('${operation} failed: ${error.message}');
      return of(result as T);
    };
  }
}
