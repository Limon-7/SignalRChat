import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sidenavHelper'
})
export class SidenavHelperPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
