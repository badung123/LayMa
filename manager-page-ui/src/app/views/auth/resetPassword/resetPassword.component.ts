import { Component,inject,OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AdminApiAuthApiClient,
  ForgotPasswordRequest,
  RegisterRequest,
  RegistrationResponse,
  ResetPasswordRequest,
} from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { Subject, takeUntil } from 'rxjs';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { UtilityService } from '../../../shared/services/utility.service';

@Component({
    selector: 'app-resetPassword',
    templateUrl: './resetPassword.component.html',
    styleUrls: ['./resetPassword.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule]
})
export class ResetPasswordComponent implements OnDestroy,OnInit{
  resetPasswordForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  constructor(private fb: FormBuilder,
    private utilService: UtilityService,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private routeParam: ActivatedRoute) { 
      this.resetPasswordForm = this.fb.group({
        password: new FormControl('', Validators.required),
        confirmPassword: new FormControl('', Validators.required)
      });
    }
    ngOnInit(): void {
    }
    
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
    resetPassword() {
      let password = this.resetPasswordForm.controls['password'].value;
      if (password == null || password == '') {
        this.alertService.showError('Mật khẩu không được để trống');
        return;
      }
      if (password.length < 8) {
        this.alertService.showError('Mật khẩu ít nhất 8 ký tự');
        return;
      }
      let confirmPassword = this.resetPasswordForm.controls['confirmPassword'].value;
      if (password != confirmPassword) {
        this.alertService.showError('Mật khẩu không giống nhau');
        return;
      }
      let email = this.routeParam.snapshot.queryParamMap.get('email');
      if (email == null || email == '') {
        this.alertService.showError('Email không đúng.Vui lòng kiểm tra lại');
        return;
      }
      if(!this.utilService.validateEmail(email)){
        this.alertService.showError('Email không đúng định dạng');
        return;
      }      
      let token = this.routeParam.snapshot.queryParamMap.get('token');
      if (token == null || token == '') {
        this.alertService.showError('Token chưa chính xác');
        return;
      }
      var request: ResetPasswordRequest = new ResetPasswordRequest({
        email: email,
        password: this.resetPasswordForm.controls['password'].value,
        token: token
      }); 
      this.authApiClient.resetPassword(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: any) => {
          //Redirect to login
          this.router.navigate([UrlConstants.LOGIN]);
        },
        error: (error: any) => {         
          this.alertService.showError(error);                
        },
      });
    }
}
