import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { PermissionService } from '@core/services/permission.service';

@Directive({
    selector: '[hasPermission]',
    standalone: true
})
export class HasPermissionDirective {
    private permissionService = inject(PermissionService);
    private viewContainer = inject(ViewContainerRef);
    private templateRef = inject(TemplateRef<any>);

    @Input() set hasPermission(permission: { page: string; operation: string }) {
        if (this.permissionService.hasPermission(permission.page, permission.operation)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}
