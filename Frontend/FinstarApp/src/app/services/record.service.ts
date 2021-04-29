import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Record } from '../record'
import { Model } from '../model'
import { map } from 'rxjs/operators';

const baseURL = 'https://localhost:44336/api/v1/records'

@Injectable({
  providedIn: 'root'
})
export class RecordService {

  constructor(private httpClient: HttpClient) { }

  readAll(): Observable<Record[]> {
    return this.httpClient.get<Model>(baseURL).pipe(map(data=>{
      return data.records
  }));
  }

  upload(body): Observable<any> {
    return this.httpClient.post(baseURL, body)
  }
}