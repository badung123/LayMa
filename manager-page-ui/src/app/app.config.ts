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
import { ADMIN_API_BASE_URL, AdminApiAuthApiClient, AdminApiTokenApiClient,AdminApiTestApiClient, AdminApiShortLinkApiClient, AdminApiKeySearchApiClient, AdminApiBankTransactionApiClient } from './api/admin-api.service.generated';
import { environment } from './../environments/environment';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AlertService } from './shared/services/alert.service';
import { TokenStorageService } from './shared/services/token-storage.service';
import { TokenInterceptor } from './shared/interceptors/token.interceptor';
import { HTTP_INTERCEPTORS,HttpClientModule } from '@angular/common/http';
import { AuthGuard } from './shared/auth.guard';
import { BroadcastService } from 'src/app/shared/services/boardcast.service';
import { UtilityService } from './shared/services/utility.service';

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
    importProvidersFrom(SidebarModule, DropdownModule,ToastModule,HttpClientModule),
    IconSetService,
    MessageService,
    AlertService,
    UtilityService,
    AdminApiAuthApiClient,
    TokenStorageService,
    AdminApiTokenApiClient,
    TokenStorageService,
    AuthGuard,
    BroadcastService,
    AdminApiTestApiClient,
    AdminApiShortLinkApiClient,
    AdminApiKeySearchApiClient,
    AdminApiBankTransactionApiClient,
    provideAnimations()
  ]
};
