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
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';


@Component({
    selector: 'app-verifyAccount',
    templateUrl: './verifyAccount.component.html',
    styleUrls: ['./verifyAccount.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class VerifyAccountComponent implements OnInit, OnDestroy{
    verifyAccountForm: FormGroup;
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 5;
    public totalCount: number;
    public isVerify = false;

    //Business variables
    public histories: BankTransactionInListDto[];
    public keyword: string = '';
    public accountName: string;
    public processStatus = ProcessStatus;
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private bankTransactionApiClient: AdminApiBankTransactionApiClient,
    ) {
      this.verifyAccountForm = this.fb.group({
        origin: new FormControl('', Validators.required),
        contact: new FormControl('', Validators.required)
      });
    }
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {   
    }
    verifyAccount(){

    }
}