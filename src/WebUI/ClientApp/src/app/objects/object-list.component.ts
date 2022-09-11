import { Component, TemplateRef, OnInit, ViewChild } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { CreateObjectCommand, ObjectBriefDto, ObjectDto, ObjectsClient, ObjectsTypesClient, ObjectTypeDto, PaginatedListOfObjectBriefDto, UpdateObjectCommand } from '../web-api-client';

@Component({
  selector: 'app-object-list',
  templateUrl: './object-list.component.html',
  styleUrls: ['./object-list.component.scss']
})
export class ObjectListComponent {
  debug = false;
  paginatedList: PaginatedListOfObjectBriefDto;
  searchTerm: string = '';
  selectedItem: ObjectBriefDto;

  itemDetailsEditor: any;
  itemDetailsModalRef: BsModalRef;
  @ViewChild('itemDetailsModalTemplate') itemDetailsModalTemplate;
  errorMsg: string = '';

  keyword = 'name';
  data: any = [];
  isLoadingResult: boolean;
  initialObjectTypeValue: any;


  @ViewChild('objectauto') objectAutocomplete;
  isLoadingObjectsResult: boolean;
  objectAutoCompleteData: ObjectBriefDto[] = [];
  selectedObject: ObjectBriefDto;

  constructor
    (private client: ObjectsClient,
      private objectTypesClient: ObjectsTypesClient,
      private modalService: BsModalService
    ) { }

  ngOnInit(): void {
    this.search();    
  }

  search(): void {
    this.client.getObjectsWithPagination(this.searchTerm, 1, 10).subscribe(result => {
      this.paginatedList = result;
    }, error => console.error(error));
  }

  getServerResponse(event) {
    this.isLoadingResult = true;
    this.objectTypesClient.getObjectTypesWithPaginationQuery(event, 1, 10).subscribe(result => {
      this.data = result.items;
    }, error => {
      console.error(error);
    },
    () => {
      this.isLoadingResult = false;
    });
  }

  addNew() {
    let newObject = new ObjectDto({ name: '', relatedObjects: []});
    this.itemDetailsEditor = newObject;
    this.initialObjectTypeValue = '';
    this.itemDetailsModalRef = this.modalService.show(this.itemDetailsModalTemplate);
  }

  
  showItemDetailsModal(template: TemplateRef<any>, item: ObjectBriefDto): void {
    this.selectedItem = item;

    this.client.getObjectDetails(item.id).subscribe(result => {
      this.itemDetailsEditor = result;
      this.initialObjectTypeValue = result.objectType;
      this.itemDetailsModalRef = this.modalService.show(template);
    }, error => console.error(error));
  }

  updateItemDetails(): void {
    const item = this.itemDetailsEditor

    const fields = {
      id: item.id,
      name: item.name,
      description: item.description,
      objectTypeId: item.objectType?.id,
      relatedObjects: item.relatedObjects.map(x => x.id)
    }
   
    if (item.id) {
      // Update existing object
      const cmd = new UpdateObjectCommand(fields);
      this.client.update(item.id, cmd).subscribe(
        () => {
          this.itemDetailsModalRef.hide();
          this.itemDetailsEditor = {};
          this.errorMsg = '',
          this.search();
        },
        error => this.handleError(error)
      );
    } else {
      // Create new object
      const cmd = new CreateObjectCommand(fields);
      this.client.create(cmd).subscribe(
        () => {
          this.itemDetailsModalRef.hide();
          this.itemDetailsEditor = {};
          this.errorMsg = '',
            this.search();
        },
        error => this.handleError(error)
      );
    }

    
  }
  handleError(error) {
    let msg = 'Error saving object';

    // This Problem Detail error handling should be an Interceptor or similar
    if (error.status === 400 && error.response) {
      let response = JSON.parse(error.response);
      if (response.title) {
        msg = response.title;
      }
      if (response.errors) {
        Object.entries(response.errors).forEach(([key, value], index) => {
          msg += " [" + value + "]";
        });
      }
    }
    this.errorMsg = msg;
    console.error(error);
  }

  deleteItem(item: ObjectBriefDto) {
    if (this.itemDetailsModalRef) {
      this.itemDetailsModalRef.hide();
    }

    if (item.id !== null) {     
      this.client.delete(item.id).subscribe(
        () => (this.paginatedList.items = this.paginatedList.items.filter(
          t => t.id !== item.id
        )),        
        error => console.error(error)
      );
    }
  }

  searchCleared() {
    this.itemDetailsEditor.objectType = null;
    this.initialObjectTypeValue = null;
    this.data = [];
  }

  selectEvent(item) {
    // do something with selected item
    this.itemDetailsEditor.objectType = item;   
  }

  onFocused(e) {
    // do something when input is focused
    this.getServerResponse('');
  }




  /* Relationships*/

  objectGetAutocompleServerResponse(event) {
    this.isLoadingObjectsResult = true;
    this.client.getObjectsWithPagination(event, 1, 10).subscribe(result => {
      this.objectAutoCompleteData = result.items;
    }, error => {
      console.error(error);
    },
    () => {
      this.isLoadingObjectsResult = false;
    });
  }

  objectSelectEvent(item) {    
    var newItem = { ...item };
    this.itemDetailsEditor.relatedObjects.push(newItem);
    console.log(this.objectAutocomplete);
    //this.objectAutocomplete.clear();
    //this.objectSearchCleared();
  }

  objectOnFocused(e) {
    this.objectGetAutocompleServerResponse('');
  }

  objectSearchCleared() {    
    this.objectAutoCompleteData = [];
  }

  deleteRelationship(item: ObjectBriefDto) {
    const itemIndex = this.itemDetailsEditor.relatedObjects.indexOf(item);
    this.itemDetailsEditor.relatedObjects.splice(itemIndex, 1);
  }

}
