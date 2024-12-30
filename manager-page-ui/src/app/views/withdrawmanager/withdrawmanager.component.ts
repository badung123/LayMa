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
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';

@Component({
    selector: 'app-withdrawmanager',
    templateUrl: './withdrawmanager.component.html',
    styleUrls: ['./withdrawmanager.component.scss'],
    standalone: true,
    imports: [ContainerComponent,PaginatorModule,DropdownComponent,DropdownItemDirective,DropdownMenuDirective,DropdownToggleDirective, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class WithDrawManagerComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 10;
    public totalCount: number;

    //Business variables
    public items: BankTransactionInListDto[];
    public keyword: string = '';
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
      this.loadData();
    }

    loadData(){
      this.bankTransactionApiClient.getAllPaging(this.pageIndex,this.pageSize,this.keyword)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: BankTransactionInListDtoPagedResult) => {
          this.items = response.results!;
          this.totalCount = response.rowCount!;
          console.log(this.items);
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
    changestatus(status: number,id: string,userId:string,money: number){
       var request: UpdateStatusRequest = new UpdateStatusRequest({
          type:status,
          id:id,
          userId:userId,
          money:money
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
}