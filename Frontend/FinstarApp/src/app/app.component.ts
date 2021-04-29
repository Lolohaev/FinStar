import { Component, OnInit } from '@angular/core';
import { RecordService } from 'src/app/services/record.service';
import {Model} from "./model";
import {Record} from "./record";
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  records: Record[]
  page: string
  records1: number[] = [1, 2, 3, 4]
  constructor(private http: HttpClient, private recordService: RecordService){ }

  ngOnInit(): void {
    this.recordService.readAll().subscribe(data => {this.records = data; console.log(this.records);});
  }

  show(): void {
    console.log(this.records[0].id)
  }
  // readRecords(): void {
  //   this.recordService.readAll()
  //     .subscribe(
  //       model => {
  //         this.records = model.records;
  //         this.page = model.pageInfo;
  //         console.log(this.records);
  //         console.log(this.page);
  //       },
  //       error => {
  //         console.log(error);
  //       });
  //}


  title = 'FinstarApp';
}
