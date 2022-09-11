import { Component } from '@angular/core';
import { WeatherForecastClient, WeatherForecast, ObjectBriefDto, ObjectsClient, PaginatedListOfObjectBriefDto } from '../web-api-client';

@Component({
  selector: 'app-object-list',
  templateUrl: './object-list.component.html'
})
export class ObjectListComponent {
  
  public paginatedList: PaginatedListOfObjectBriefDto;
  public searchTerm: string = '';

  constructor(private client: ObjectsClient) {
    client.getObjectsWithPagination("",1,10).subscribe(result => {
      this.paginatedList = result;
    }, error => console.error(error));
  }

  search(): void {
    this.client.getObjectsWithPagination(this.searchTerm, 1, 10).subscribe(result => {
      this.paginatedList = result;
    }, error => console.error(error));
  }
}
