import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'joinNames', standalone: true })
export class JoinNamesPipe implements PipeTransform {
  transform(value: { name: string }[] | null): string {
    return value?.map(x => x.name).join(', ') ?? '';
  }
}

