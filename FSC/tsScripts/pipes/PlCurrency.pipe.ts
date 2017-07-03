import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'plCurrency' })
export class PlCurrencyPipe implements PipeTransform {

    transform(value: number): string {
        var parts = value.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, " ");
        return parts.join(",")+" PLN";
    }
}