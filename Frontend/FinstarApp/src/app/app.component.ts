import { Component, OnInit } from '@angular/core';
import { RecordService } from 'src/app/services/record.service';
import {Record} from "./record";
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  records: Record[]
  page: string
  fileToUpload: File

  constructor(private http: HttpClient, private recordService: RecordService){ }

  ngOnInit(): void {
    this.getData()
  }

  getData(): void {
    this.recordService.readAll().subscribe(data => {this.records = data; console.log(this.records);});
  }

  uploadData(data): void {
    this.recordService.upload(data).subscribe(data => console.log(data))
  }

  handleFileInput(event) {
    let input = event.target;
    for (var index = 0; index < input.files.length; index++) {
        let reader = new FileReader();
        reader.onload = () => {
            var text = reader.result;
            this.uploadData(text)
        }
        reader.readAsText(input.files[index]);
    };
}

  title = 'FinstarApp';
}
