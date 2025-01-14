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
import { AdminApiBankTransactionApiClient, BankTransactionInListDto, BankTransactionInListDtoPagedResult,ProcessStatus, UpdateStatusRequest } from '../../api/admin-api.service.generated';
import { CommonModule, NgStyle } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  NgModel,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { PaginatorModule } from 'primeng/paginator';
import { CalendarModule } from 'primeng/calendar';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';
interface IStatus {
  id: number,
  name: string,
}
@Component({
    selector: 'app-withdrawmanager',
    templateUrl: './withdrawmanager.component.html',
    styleUrls: ['./withdrawmanager.component.scss'],
    standalone: true,
    imports: [ContainerComponent,PaginatorModule,CalendarModule,DropdownComponent,DropdownItemDirective,DropdownMenuDirective,DropdownToggleDirective, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class WithDrawManagerComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 10;
    public totalCount: number;
    public userNameSearch: string = '';
    public from: Date;
    public to: Date;
    public rangeDates: Date[] = [new Date(),new Date()];
    public listAdmin: string[] = ['All','ketoan83','admin83'];
    public adminSearch: string = 'All';
    public listStatus: IStatus[] =[
      {id:-1,name:'All'},
      {id:0,name:'Chờ xử lý'},
      {id:1,name:'Đang xử lý'},
      {id:2,name:'Từ chối'},
      {id:3,name:'Thành công'}
    ];
    public statusSearch: number = -1;

    //Business variables
    public items: BankTransactionInListDto[];
    public accountName: string;
    public processStatus = ProcessStatus;
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private bankTransactionApiClient: AdminApiBankTransactionApiClient,
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
      this.bankTransactionApiClient.getAllPaging(this.from,this.to,this.pageIndex,this.pageSize,this.userNameSearch,this.adminSearch,this.statusSearch)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: BankTransactionInListDtoPagedResult) => {
          this.items = response.results!;
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
    changestatus(status: number,id: string,userId:string,money: number,modifiedBy:string = ""){
       var request: UpdateStatusRequest = new UpdateStatusRequest({
          type:status,
          id:id,
          userId:userId,
          money:money,
          modifiedBy: modifiedBy
        });
      this.bankTransactionApiClient.updateProcessStatus(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: any) => {
          this.loadData();
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    changeDateRange(event){
      if (event[1] != null) {
        this.from = event[0];
        this.to = event[1];
        this.loadData();
      }     
    }
}