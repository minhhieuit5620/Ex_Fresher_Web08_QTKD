$(document).ready(function() {
    $(document).on('click', '.combobox #icon__combobox', btnComboboxOnClick);
    $(document).on('click', '.combobox .combobox__item', comboboxItemOnSelect);

    // tìm tất cả các element là mcombobox:
    var comboboxs = $("mcombobox");
    // Build từng combobox:
    for (const cbx of comboboxs) {
        // Lấy Id:
        const id = cbx.getAttribute("id");
        // Lấy ra api:
        const api = cbx.getAttribute("api");
        // Lấy ra propText:
        const text = cbx.getAttribute("text");
        // Lấy ra propValue:
        const value = cbx.getAttribute("value");


        // Lấy dữ liệu dựa vào api:
        $.ajax({
            type: "GET",
            url: api,
            async: false,
            success: function(res) {
                console.log(cbx);
                let comboboxHTML = $(`<div id="${id}" class="combobox">
                                        <input type="text">
                                       <div class="combox__drop icon" id="icon__combobox">                                       
                                        </div>
                                        <div class="combobox__data" hidden>
                                        </div>
                                    </div>`);
                for (const item of res) {
                    let html = `<div class="combobox__item" value="${item[value.trim()]}">${item[text.trim()]}</div>`;
                    $(comboboxHTML).find(".combobox__data").append(html);
                    // Thay thế html của combobox:
                }
                $(comboboxHTML).data("data", res);
                cbx.replaceWith(comboboxHTML[0]);
            }
        });
    }
})

function btnComboboxOnClick() {
    $(this.nextElementSibling).toggle();
}

function comboboxItemOnSelect() {
    // Lấy ra text và value vừa chọn:
    // debugger
    const text = this.textContent;
    const value = this.getAttribute("value");
    console.log(value);
    // Binding text lên input:
    let parentElement = this.parentElement;
    let input = $(parentElement).siblings("input");
    // input.value = text;
    $(input).val(text);
    $(parentElement).hide();
    // Lưu lại value vừa chọn:
    $(parentElement.parentElement).data("value", value);
}