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
  RegisterRequest,
  RegistrationResponse,
} from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { Subject, takeUntil } from 'rxjs';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { UtilityService } from '../../../shared/services/utility.service';
import { environment } from '../../../../environments/environment';
import { forEach } from 'lodash-es';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    standalone: true,
    imports: [ContainerComponent,CommonModule, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective,ReactiveFormsModule]
})
export class RegisterComponent implements OnDestroy,OnInit{
  registerForm: FormGroup;
  offRegister = true;
  private ngUnsubscribe = new Subject<void>();
  constructor(private fb: FormBuilder,
    private utilService: UtilityService,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private routeParam: ActivatedRoute) { 
      this.registerForm = this.fb.group({
        userName: new FormControl('', Validators.required),
        password: new FormControl('', Validators.required),
        refcode: new  FormControl(''),
        email: new FormControl('', Validators.required),
        confirmPassword: new FormControl('', Validators.required),
      });
    }
    ngOnInit(): void {
      this.offRegister = environment.OFF_REGISTER;
    }
    
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
    dangnhap(){
      this.router.navigate([UrlConstants.LOGIN]);
    }
    register() {
      let userName = this.registerForm.controls['userName'].value;
      if (userName == null || userName == '') {
        this.alertService.showError('Tên đăng nhập không được để trống');
        return;
      }
      let email = this.registerForm.controls['email'].value;
      if (email == null || email == '') {
        this.alertService.showError('Email không được để trống');
        return;
      }
      let refcode = this.routeParam.snapshot.queryParamMap.get('refcode');
      if(!this.utilService.validateEmail(email)){
        this.alertService.showError('Email không đúng định dạng');
        return;
      }
      let password = this.registerForm.controls['password'].value;
      if (password == null || password == '') {
        this.alertService.showError('Mật khẩu không được để trống');
        return;
      }
      if (password.length < 8) {
        this.alertService.showError('Mật khẩu ít nhất 8 ký tự');
        return;
      }
      let confirmPassword = this.registerForm.controls['confirmPassword'].value;
      if (password != confirmPassword) {
        this.alertService.showError('Mật khẩu không giống nhau');
        return;
      }

      var request: RegisterRequest = new RegisterRequest({
        userName: this.registerForm.controls['userName'].value,
        password: this.registerForm.controls['password'].value,
        email: this.registerForm.controls['email'].value,
        refcode: refcode == null ? '' : refcode,
        confirmPassword: this.registerForm.controls['confirmPassword'].value,
      }); 
      this.authApiClient.register(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: RegistrationResponse) => {
          //Redirect to dashboard
          this.router.navigate([UrlConstants.LOGIN]);
  
        },
        error: (error: RegistrationResponse) => {         
          if (error != null) {
            error.errors!.forEach(description => {
              this.alertService.showError(description);
            });  
          }
                 
        },
      });
    }
}
