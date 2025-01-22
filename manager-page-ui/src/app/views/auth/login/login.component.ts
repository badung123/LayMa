import { Component,OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import {
  AdminApiAuthApiClient,
  AuthenticatedResult,
  LoginRequest,
} from 'src/app/api/admin-api.service.generated';
import { AlertService } from 'src/app/shared/services/alert.service';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { Subject, takeUntil } from 'rxjs';
import { NgStyle,CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { BroadcastService } from 'src/app/shared/services/boardcast.service';
import { environment } from '../../../../environments/environment';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, NgStyle,ReactiveFormsModule,CommonModule]
})
export class LoginComponent implements OnDestroy,OnInit{
  loginForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  loading = false;
  offRegister = true;
  constructor(
    private fb: FormBuilder,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private tokenSerivce: TokenStorageService,
    private broadCastService: BroadcastService
  ) { 
    this.loginForm = this.fb.group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }
  ngOnInit(): void {
    this.offRegister = environment.OFF_REGISTER;
    this.broadCastService.httpError.asObservable().subscribe(values => {
      this.loading = false;
  });
  }
  
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  login() {
    this.loading = true;
    var request: LoginRequest = new LoginRequest({
      userName: this.loginForm.controls['userName'].value,
      password: this.loginForm.controls['password'].value,
    });

    this.authApiClient.login(request)
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe({
      next: (res: AuthenticatedResult) => {
        //Save token and refresh token to localstorage
        this.tokenSerivce.saveToken(res.token);
        this.tokenSerivce.saveRefreshToken(res.refreshToken);
        this.tokenSerivce.saveUser(res);
        //Redirect to dashboard
        var loggedInUser = this.tokenSerivce.getUser();
        if (loggedInUser) {
          if (loggedInUser.roles.includes("User")) {
            this.router.navigate([UrlConstants.HOME]);
          }
          if (loggedInUser.roles.includes("Admin")) {
            this.router.navigate([UrlConstants.CAMPAIN]);
          }
          if (loggedInUser.roles.includes("Accountant")) {
            this.router.navigate([UrlConstants.WITHDRAWMANAGER]);
          }
        }
        

      },
      error: (error: any) => {
        console.log(error);
        this.alertService.showError(error);
        this.loading = false;
      },
    });
  }
  clickRegister(){
    this.router.navigate([UrlConstants.REGISTER]);
  }
  clickForgetPassword(){
    this.router.navigate([UrlConstants.FORGETPASSWORD]);
  }

}
