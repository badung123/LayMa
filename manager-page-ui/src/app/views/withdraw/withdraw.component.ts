import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective, CardHeaderComponent, TableDirective, FormFloatingDirective, FormLabelDirective, FormSelectDirective } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { AdminApiBankTransactionApiClient, BankTransactionInListDto, BankTransactionInListDtoPagedResult, CreateBankTransactionDto,ProcessStatus } from '../../api/admin-api.service.generated';
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
import { UtilityService } from 'src/app/shared/services/utility.service';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';
interface IBank {
  id:number,
  name: string;
  shortName: string;
}

@Component({
    selector: 'app-withdraw',
    templateUrl: './withdraw.component.html',
    styleUrls: ['./withdraw.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class WithDrawComponent implements OnInit, OnDestroy{
    withdrawForm: FormGroup;
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 5;
    public totalCount: number;

    //Business variables
    public histories: BankTransactionInListDto[];
    public keyword: string = '';
    public selectedBank: IBank;
    public accountName: string;
    public processStatus = ProcessStatus;
    constructor(private fb: FormBuilder,
      private utilService: UtilityService,
      private router: Router,
      private alertService: AlertService,
      private bankTransactionApiClient: AdminApiBankTransactionApiClient,
    ) {
      this.withdrawForm = this.fb.group({
        accountNumber: new FormControl('', Validators.required),
        amount: new FormControl('', Validators.required)
      });
    }
    public banks: IBank[] = [
      {
        id:1,
        name:'Ngân hàng Quân Đội',
        shortName:'MB'
      },
      {
        id:2,
        name:'Ngân hàng TMCP Kỹ Thương Việt Nam',
        shortName:'TCB'
      },
      {
        id:3,
        name:'Ngân hàng TMCP Ngoại thương Việt Nam',
        shortName:'VCB'
      } 
    ];
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {
      this.loadData();    
    }
    withdraw() {
      if (this.selectedBank == null || this.selectedBank.shortName == null) {
        this.alertService.showError('Bạn cần chọn ngân hàng');
        return;
      }
      if (this.accountName == null || this.accountName == '') {
        this.alertService.showError('Bạn cần nhập tên tài khoản');
        return;
      }
      
      var request: CreateBankTransactionDto = new CreateBankTransactionDto({
        bankAccountName: this.accountName,
        bankAccountNumber:this.withdrawForm.controls['accountNumber'].value.toString(),
        money:this.withdrawForm.controls['amount'].value,
        bankName:this.selectedBank.shortName
      });
      console.log(request);
  
      this.bankTransactionApiClient.createBankTransaction(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          //Redirect to dashboard
          this.loadData();
  
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    loadData() {
      this.bankTransactionApiClient.getPostsPaging(this.pageIndex,this.pageSize,this.keyword)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: BankTransactionInListDtoPagedResult) => {
          this.histories = response.results!;
          console.log(this.histories);
          this.totalCount = response.rowCount!;
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }

    validateName(value){
      this.accountName = this.utilService.toNonAccentVietnamese(value);
    }

    
    

}