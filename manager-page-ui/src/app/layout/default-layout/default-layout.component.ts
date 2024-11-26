import { Component,OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { NgScrollbar } from 'ngx-scrollbar';

import { IconDirective } from '@coreui/icons-angular';
import {
  ContainerComponent,
  ShadowOnScrollDirective,
  SidebarBrandComponent,
  SidebarComponent,
  SidebarFooterComponent,
  SidebarHeaderComponent,
  SidebarNavComponent,
  SidebarToggleDirective,
  SidebarTogglerDirective
} from '@coreui/angular';

import { DefaultFooterComponent, DefaultHeaderComponent } from './';
import { navItems } from './_nav';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { Router } from '@angular/router';
import { UrlConstants } from 'src/app/shared/constants/url.constants';

function isOverflown(element: HTMLElement) {
  return (
    element.scrollHeight > element.clientHeight ||
    element.scrollWidth > element.clientWidth
  );
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
  standalone: true,
  imports: [
    SidebarComponent,
    SidebarHeaderComponent,
    SidebarBrandComponent,
    RouterLink,
    IconDirective,
    NgScrollbar,
    SidebarNavComponent,
    SidebarFooterComponent,
    SidebarToggleDirective,
    SidebarTogglerDirective,
    DefaultHeaderComponent,
    ShadowOnScrollDirective,
    ContainerComponent,
    RouterOutlet,
    DefaultFooterComponent
  ]
})
export class DefaultLayoutComponent implements OnInit{
  public navItems = navItems;
  constructor(
    private tokenService: TokenStorageService,
    private router: Router
  ) {}
  onScrollbarUpdate($event: any) {
    // if ($event.verticalUsed) {
    // console.log('verticalUsed', $event.verticalUsed);
    // }
  }
  ngOnInit(): void {
    var user = this.tokenService.getUser();
    if (user == null) this.router.navigate([UrlConstants.LOGIN]);
    var permissions = JSON.parse(user.permissions);
    for (var index = 0; index < navItems.length; index++) {
      for (
        var childIndex = 0;
        childIndex < navItems[index].children?.length;
        childIndex++
      ) {
        if (
          navItems[index].children[childIndex].attributes &&
          permissions.filter(
            (x) =>
              x == navItems[index].children[childIndex].attributes['policyName']
          ).length == 0
        ) {
          navItems[index].children[childIndex].class = 'hidden';
        }
      }
    }
    this.navItems = navItems;
  }
}
