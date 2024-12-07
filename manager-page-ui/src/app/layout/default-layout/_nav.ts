import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Thống kê',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' },
    badge: {
      color: 'info',
      text: 'NEW'
    },
    attributes: {
      "policyName": "Permissions.Dashboard.View"
    }
  },
  {
    name: 'Link Rút Gọn',
    url: '/shortlink/list',
    iconComponent: { name: 'cil-cursor' },
    children: [
      {
        name: 'Tạo link rút gọn',
        url: '/shortlink/create',
        icon: 'nav-icon-bullet',
        attributes: {
          "policyName": "Permissions.ShortLink.Create"
        }
      },
      {
        name: 'Quản lý link rút gọn',
        url: '/shortlink/list',
        icon: 'nav-icon-bullet',
        attributes: {
          "policyName": "Permissions.ShortLink.View"
        }
      }
    ]
  },
  {
    name: 'Rút tiền',
    url: '/withdraw',
    iconComponent: { name: 'cil-puzzle' },
    badge: {
      color: 'info',
      text: 'NEW'
    },
    attributes: {
      "policyName": "Permissions.WithDraw.Create"
    }
  }
];
