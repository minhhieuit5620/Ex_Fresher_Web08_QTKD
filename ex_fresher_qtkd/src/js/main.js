$(document).ready(function() {
    // $(document).on( 'blur', btnSelectClose);
    $(document).on('click', '#icon__edit--drop', btnSelectOnClick);
    $(document).on('click','.btn__cancel','.dialog__wrap',btnCloseDialog);
    
    // $(document).on('click','.content',btnCloseDialog);
    $(document).on('click','.add__emp',btnAdd);
    $(document).on('click','.btn__aprove',btnAprove);
    $(document).on('click','.btn__close ',btnClose);
    $(document).on('click','.dialog__close ',iconClose);

    
})
function btnSelectOnClick() {    
    $(this.nextElementSibling).toggle();
   
    // if(this.nextElementSibling.show){
    //     $('#icon__edit--drop').css('border','1px solid blue');
    // }
    // else{
    //     $('#icon__edit--drop').css('border','none');
    // }
}
function iconClose(){
    $('.dialog__wrap').hide();
}

function btnCloseDialog(){
    $('.dialog__wrap').hide();
}
function btnAdd(){
    
    // alert("show")
    $('.dialog__wrap').show();
}
function btnAprove(){
    $('.wrap__waring').hide();
}
function btnClose(){
    $('.wrap__error').hide();  
    $('.wrap__ask').hide();

}



