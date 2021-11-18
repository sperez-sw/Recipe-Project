import { Directive, HostBinding, HostListener } from "@angular/core";

@Directive({
    selector:'[appDropdown]'
})
export class DropdownDirective{
    @HostBinding('class.open') isOpen = false;
    @HostListener('click') toggleOpen(eventData : Event){
        this.isOpen = !this.isOpen;
    }
    //EN CASO DE QUE SE QUIERA CERRAR LA DIRECTIVA HACIENDO CLICK FUERA DEL TAG
    // @HostListener('document:click', ['$event']) toggleOpen(event: Event) {
    //     this.isOpen = this.elRef.nativeElement.contains(event.target) ? !this.isOpen : false;
    //   }
    //   constructor(private elRef: ElementRef) {}
}