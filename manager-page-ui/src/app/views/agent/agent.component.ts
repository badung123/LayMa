import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective, CardHeaderComponent, TableDirective, FormFloatingDirective, FormLabelDirective, FormSelectDirective } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { AdminApiUserApiClient, AgentListDto, AgentListDtoPagedResult, ShortLinkInListDtoPagedResult } from '../../api/admin-api.service.generated';
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
import { UrlConstants } from '../../shared/constants/url.constants';
import { TokenStorageService } from '../../shared/services/token-storage.service';
import { Clipboard } from "@angular/cdk/clipboard";
import { PaginatorModule } from 'primeng/paginator';
//import { AdminApiShortLinkApiClient, AdminApiTestApiClient, CreateShortLinkDto } from 'src/app/api/admin-api.service.generatesrc';


@Component({
    selector: 'app-agent',
    templateUrl: './agent.component.html',
    styleUrls: ['./agent.component.scss'],
    standalone: true,
    imports: [ContainerComponent,PaginatorModule, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective]
})
export class AgentComponent implements OnInit, OnDestroy{
    //System variables
    private ngUnsubscribe = new Subject<void>();
    //Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 10;
    public totalCount: number;
    public btn1text: string = "Sao chép";

    //Business variables
    public listAgent: AgentListDto[];
    public keyword: string = '';
    public linkRef: string;
    constructor(private fb: FormBuilder,
      private router: Router,
      private clipboard: Clipboard,
      private alertService: AlertService,
      private userApiClient: AdminApiUserApiClient,
      private tokenService: TokenStorageService
    ) {
    }
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {
      var loggedInUser = this.tokenService.getUser();
      if (loggedInUser) {
        this.linkRef = UrlConstants.REFCODE_URL + loggedInUser.code;
      }
      this.loadData();
    }
    loadData(){
      this.userApiClient.getListAgentByUserId(this.pageIndex,this.pageSize,this.keyword)
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe({
              next: (response: AgentListDtoPagedResult) => {
                this.listAgent = response.results!;
                console.log(this.listAgent);
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
    copyToClipboard() {
      this.clipboard.copy(this.linkRef);
      this.btn1text = "Đã sao chép";
      setTimeout(() => {
        this.btn1text = "Sao chép";
      }, 2000);
    }
}