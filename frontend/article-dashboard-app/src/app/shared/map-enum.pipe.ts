import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'mapEnum', standalone: true })
export class MapEnumPipe implements PipeTransform {
  transform(value: number | number[] | null | undefined, enumMap: Record<number, string>): string {
    if (value == null) return '';
    if (Array.isArray(value)) {
      return value.map(v => enumMap[v] ?? v).join(', ');
    }
    return enumMap[value] ?? String(value);
  }
}