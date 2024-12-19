import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  provideRouter,
  withEnabledBlockingInitialNavigation,
  withHashLocation,
  withInMemoryScrolling,
  withRouterConfig,
  withViewTransitions
} from '@angular/router';

import { DropdownModule, SidebarModule } from '@coreui/angular';
import { IconSetService } from '@coreui/icons-angular';
import { routes } from './app.routes';
import { ADMIN_API_BASE_URL, AdminApiAuthApiClient, AdminApiTokenApiClient,AdminApiTestApiClient, AdminApiShortLinkApiClient, AdminApiKeySearchApiClient, AdminApiBankTransactionApiClient, AdminApiCampainApiClient, AdminApiUserApiClient } from './api/admin-api.service.generated';
import { environment } from './../environments/environment';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AlertService } from './shared/services/alert.service';
import { TokenStorageService } from './shared/services/token-storage.service';
import { TokenInterceptor } from './shared/interceptors/token.interceptor';
import { HTTP_INTERCEPTORS,HttpClientModule } from '@angular/common/http';
import { AuthGuard } from './shared/auth.guard';
import { BroadcastService } from './shared/services/boardcast.service';
import { UtilityService } from './shared/services/utility.service';
import { UploadService } from './shared/services/upload.service';
import { DialogService, DynamicDialogModule } from 'primeng/dynamicdialog';
import { CalendarModule } from 'primeng/calendar';

export const appConfig: ApplicationConfig = {
  providers: [
    { provide: ADMIN_API_BASE_URL, useValue: environment.API_URL },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    provideRouter(routes,
      withRouterConfig({
        onSameUrlNavigation: 'reload'
      }),
      withInMemoryScrolling({
        scrollPositionRestoration: 'top',
        anchorScrolling: 'enabled'
      }),
      withEnabledBlockingInitialNavigation(),
      withViewTransitions(),
      withHashLocation()
    ),    
    importProvidersFrom(SidebarModule, DropdownModule,ToastModule,HttpClientModule,DynamicDialogModule),
    IconSetService,
    MessageService,
    AlertService,
    UtilityService,
    UploadService,
    DialogService,
    AdminApiAuthApiClient,
    AdminApiUserApiClient,
    TokenStorageService,
    AdminApiTokenApiClient,
    TokenStorageService,
    AuthGuard,
    BroadcastService,
    AdminApiTestApiClient,
    AdminApiShortLinkApiClient,
    AdminApiKeySearchApiClient,
    AdminApiCampainApiClient,
    AdminApiBankTransactionApiClient,
    provideAnimations()
  ]
};
