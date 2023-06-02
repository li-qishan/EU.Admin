/**
 * Ant Design Pro v4 use `@ant-design/pro-layout` to handle Layout.
 * You can view component api by:
 * https://github.com/ant-design/ant-design-pro-layout
 */
import type { MenuDataItem } from '@ant-design/pro-layout';
import React from 'react';
import type * as H from 'history-with-query';
import { useIntl, useLocation } from 'umi';
import _isArray from 'lodash/isArray';
import memoizedOne from 'memoize-one';
import deepEqual from 'fast-deep-equal';
import type { Route } from '@ant-design/pro-layout/lib/typings';

import type { Mode, RouteConfig } from 'use-switch-tabs';
import { isSwitchTab } from 'use-switch-tabs';
import type { SwitchTabsProps } from '@/components/SwitchTabs';
import SwitchTabs from '@/components/SwitchTabs';
import PageLoading from '@/components/PageLoading';

export interface MakeUpRoute extends Route, Pick<RouteConfig, 'follow'> {}

/** 根据路由定义中的 name 本地化标题 */
function localeRoutes(
  routes: MakeUpRoute[],
  formatMessage: any,
  parent: MakeUpRoute | null = null,
): MenuDataItem[] {
  const result: MenuDataItem[] = [];

  routes.forEach((item) => {
    const { routes: itemRoutes, ...rest } = item;

    if (!item.name) {
      return;
    }

    // 初始化 locale 字段
    let newItem: MenuDataItem = {
      ...rest,
      routes: undefined,
      locale: item.name,
    };

    const localeId = parent ? `${parent.locale}.${newItem.locale}` : `${newItem.locale}`;

    newItem = {
      ...rest,
      locale: localeId,
      name: formatMessage({ id: localeId }),
    };

    if (_isArray(itemRoutes) && itemRoutes.length) {
      newItem = {
        ...newItem,
        children: localeRoutes(itemRoutes, formatMessage, newItem),
      };
    }

    result.push(newItem);
  });
  return result;
}

const memoizedOneLocaleRoutes = memoizedOne(localeRoutes, deepEqual);

export interface RouteTabsLayoutProps
  extends Pick<SwitchTabsProps, 'persistent' | 'fixed' | 'setTabName' | 'footerRender'> {
  mode?: Mode | false;
  loading?: boolean;
  routes?: MakeUpRoute[];
  children: React.ReactElement;
}

export default function SwitchTabsLayout(props: RouteTabsLayoutProps): JSX.Element {
  const { mode, loading, routes, children, ...rest } = props;

  const { formatMessage } = useIntl();
  const location = useLocation() as H.Location;
  const originalRoutes = memoizedOneLocaleRoutes(routes!, formatMessage);

  let index = originalRoutes.findIndex((x) => x.path === location.pathname);

  if (index < 0) location.pathname = '/404';

  if (mode) {
    if (loading) {
      return <PageLoading />;
    }
    if (routes) {
      return (
        <SwitchTabs
          mode={mode}
          {...rest}
          originalRoutes={originalRoutes}
          // animated={false}
        >
          {children}
        </SwitchTabs>
      );
    }
  }
  return children;
}
