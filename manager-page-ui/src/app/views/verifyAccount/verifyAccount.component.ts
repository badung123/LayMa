import { Component, OnDestroy, OnInit } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective, CardHeaderComponent, TableDirective, FormFloatingDirective, FormLabelDirective, FormSelectDirective } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { CommonModule, NgStyle } from '@angular/common';
import { UploadService } from '../../shared/services/upload.service';
import { environment } from 'src/environments/environment';
import { ImageModule } from 'primeng/image';
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
import { AdminApiUserApiClient, VerifyUserInfo, VerifyUserRequest } from '../../api/admin-api.service.generated';


@Component({
    selector: 'app-verifyAccount',
    templateUrl: './verifyAccount.component.html',
    styleUrls: ['./verifyAccount.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent,CardHeaderComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,CommonModule,ReactiveFormsModule,FormsModule,TableDirective,FormFloatingDirective,FormControlDirective, FormLabelDirective,FormSelectDirective,ImageModule]
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
    public isWaitingVerify = false;
    public contact : string = "";
    public origin : string = "";

    //Business variables
    public keyword: string = '';
    public accountName: string;
    public thumbnailImage;
    constructor(private fb: FormBuilder,
      private router: Router,
      private alertService: AlertService,
      private userApiClient: AdminApiUserApiClient,
      private uploadService: UploadService
    ) {
      this.verifyAccountForm = this.fb.group({
        origin: new FormControl('', Validators.required),
        contact: new FormControl('', Validators.required),
        thumbnail: new FormControl(
          null
        ),
      });
    }
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  
    ngOnInit() {  
      this.loadData();
    }
    onFileChange(event) {
      if (event.target.files && event.target.files.length) {
        this.uploadService.uploadImage('posts', event.target.files)
          .subscribe({
            next: (response: any) => {
              console.log(environment.API_URL + response.path)
              this.verifyAccountForm.controls['thumbnail'].setValue(response.path);
              this.thumbnailImage = environment.API_URL + response.path;
            },
            error: (err: any) => {
              console.log(err);
            }
          });
      }
    }
    loadData(){
      this.userApiClient.getInfoVerify()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: VerifyUserInfo) => {
          this.isVerify = response.isVerify!;
          this.isWaitingVerify = response.isWaitingVerify!;
          this.thumbnailImage = environment.API_URL + response.thumnail;
          this.contact = response.contact!;
          this.origin = response.origin!;
          //reload
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
    verifyAccount(){
      var request: VerifyUserRequest = new VerifyUserRequest({
        origin : this.verifyAccountForm.controls['origin'].value,
        contact : this.verifyAccountForm.controls['contact'].value,
        thumbnail : this.verifyAccountForm.controls['thumbnail'].value
      });
      this.userApiClient.verifyUser(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: any) => {
          this.alertService.showSuccess('Đã gửi yêu cầu xác minh');
          //reload
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.showError('Có lỗi xảy ra');
        },
      });
    }
}