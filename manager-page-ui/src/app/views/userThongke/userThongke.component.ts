import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, 
  RowComponent, 
  ColComponent, 
  TextColorDirective, 
  CardComponent, 
  CardBodyComponent, 
  FormDirective, 
  InputGroupComponent, 
  InputGroupTextDirective, 
  FormControlDirective, 
  ButtonDirective, 
  CardHeaderComponent, 
  TableDirective, 
  FormFloatingDirective, 
  FormLabelDirective, 
  FormSelectDirective,
  DropdownComponent,
  DropdownItemDirective,
  DropdownMenuDirective,
  DropdownToggleDirective,
 } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { AdminApiShortLinkApiClient, AdminApiUserApiClient,ProcessStatus, ThongKeViewClickByUser, ThongKeViewClickByUserPagedResult, UserDtoInList, UserDtoInListPagedResult } from '../../api/admin-api.service.generated';
import { CommonModule, NgStyle } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  NgModel,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { PaginatorModule } from 'primeng/paginator';
import { CalendarModule } from 'primeng/calendar';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';


@Component({
    templateUrl: './userThongke.component.html',
    standalone: true,
    imports: [ContainerComponent,CalendarModule,DropdownItemDirective,PaginatorModule,DropdownMenuDirective,DropdownToggleDirective, RowComponent,DropdownComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class UserThongkeComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 100;
    public totalCount: number;

    //Business variables
    public items: ThongKeViewClickByUser[];
    public userNameSearch: string = '';
    public accountName: string;
    public processStatus = ProcessStatus;
    public from: Date;
    public to: Date;
    public rangeDates: Date[] = [new Date(),new Date()];
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private userApiClient: AdminApiUserApiClient,
      private shortLinkApiClient: AdminApiShortLinkApiClient,
      public dialogService: DialogService
    ) {
    }
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() { 
      this.rangeDates[0].setHours(0,0,0,0);
      this.from = this.rangeDates[0];
      this.rangeDates[1].setHours(0,0,0,0);
      this.rangeDates[1].setDate(this.rangeDates[1].getDate() + 1)
      this.to = this.rangeDates[1]; 
      this.loadData();  
    }
    loadData(){
      console.log(this.from);
      console.log(this.to);
      this.shortLinkApiClient.getThongKeClickUserByDate(this.from,this.to,this.pageIndex,this.pageSize,this.userNameSearch)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ThongKeViewClickByUserPagedResult) => {
          this.items = response.results!;
          console.log(this.items);
          this.totalCount = response.rowCount!;
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    pageChanged(event: any): void {
      this.pageIndex = event.page + 1;
      this.pageSize = event.rows;
      this.loadData();
    }
    changeDateRange(event){
      if (event[1] != null) {
        this.from = event[0];
        this.to = event[1];
        this.loadData();
      }     
    }
    
}