import { Component, OnInit } from '@angular/core';
import { AppointmentService} from "../../services/appointment.service";
import {Appointment} from "../../models/appointment";

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.css']
})
export class AppointmentComponent implements OnInit {
  public appointment: Appointment;
  public icalendar: string;

  constructor(private appointmentService: AppointmentService) { }

  ngOnInit() {

  }

  public loadAppointment() {
    this.appointmentService.getAppointment(1)
      .subscribe(item => this.appointment = item);
  }

  public loadICalendar() {
    this.appointmentService.getICalendar(1)
      .subscribe(res =>
      {
        this.icalendar = res;
        console.log("got ical data: " + res);
      });
  }
}
