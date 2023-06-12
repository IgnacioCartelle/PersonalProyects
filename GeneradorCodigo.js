function validarInfo() {
    var rb = document.getElementsByName("TIPO");
    var id = document.getElementById("gtin").value.length;
    var err = document.getElementById("textError");
    var errIni = document.getElementById("textErrorIni");
    var errIniQR = document.getElementById("textErrorIniQR");
    var isChecked = false;
    var infoMAX = "";
    for (var i = 0; i < rb.length; i++) {
        if (rb[i].checked) {
            isChecked = true;

            infoMAX = rb[i].getAttribute("max");
        }
    }
    if (infoMAX == "") {
        var f = document.getElementById("submitBtnSI");
        f.setAttribute('disabled', '');
        errIni.style.display = "block";
        f.opacity = 0.5;
        
    }
    else {



        if (id < parseInt(infoMAX)) {
            var f = document.getElementById("submitBtnSI");
            f.setAttribute('disabled', '');
            err.style.display = "block";
            errIni.style.display = "none";
            errIniQR.style.display = "none";
            f.style.opacity = 0.5;
        } else if (id == parseInt(infoMAX)) {
            var f = document.getElementById("submitBtnSI");
            f.removeAttribute('disabled', '');
            f.style.opacity = 1;
            err.style.display = "none";
        }
        else if (infoMAX == "max" && id <= 0) {
            var f = document.getElementById("submitBtnSI");
            f.setAttribute('disabled', '');

            err.style.display = "none";
            f.style.opacity = 0.5;
            errIniQR.style.display = "block";
            errIni.style.display = "none";
        }
        else if (infoMAX == "max" && id > 0) {
            var f = document.getElementById("submitBtnSI");
            f.removeAttribute('disabled', '');
            f.style.opacity = 1;
            err.style.display = "none";
            errIni.style.display = "none";
            errIniQR.style.display = "none";


        }
    }

    if (!isChecked) {
        document.getElementById("submitBtn").disabled = "true";
    }
}

const popup = document.getElementById("popUpDescription");


$(document).on('click', '#openPopup', function () {  
    $('#modalPopup').modal();
});




popup.addEventListener("click", function (event) {
    if (event.target === popup) {
        popup.classList.add("popup-hide");
   
        setTimeout(function () {
            popup.style.display = "none";
            popup.classList.remove("popup-hide");
        }, 500);
    }
});


function popUpHide() {
    popup.style.display = "none";
}
function openPopUpDownload() {
    document.getElementById("popUpDownload").classList.toggle("popUpDownloadFormatShow")
}

function openPopUpInfo() {
    document.getElementById("infoparams").classList.toggle("aditional-params-info-show")
}

function enforceMinMax(el) {
    if (el.value != "") {
        if (parseInt(el.value) < parseInt(el.min)) {
            el.value = el.min;
        }
        if (parseInt(el.value) > parseInt(el.max)) {
            el.value = el.max;
        }
    }
}

function errorButton()
{
    if (error != "") {
        return document.getElementById("buttonError").textContent = "ENTRO JS";
    }
    else {
        console.log(error);
        return document.getElementById("buttonError").textContent = "DESCARGAR";

    }
    
}

// Define un objeto con los textos asociados a cada radio button
var res = document.getElementById("resultado");
var f;
function mostrarTexto(rb) {
    if (rb.checked) {


        var input = document.getElementById("gtin");

        var textoSeleccionado = document.getElementById("resultado");
        textoSeleccionado.innerHTML = rb.nextSibling.textContent.trim();
        input.value = "";
        validarInfo();
        var typeV = rb.getAttribute("id");
        var maxL = rb.getAttribute("max");
        f = maxL;
        if (typeV == "number") {

            input.setAttribute("maxlength", maxL)
            input.setAttribute("oninput", "javascript:  if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);this.value = this.value.replace(/[^0-9]/gi, ''); validarInfo();");
          f

        }
        else {
            input.setAttribute("type", typeV);
            input.removeAttribute("maxlength")
            input.removeAttribute("oninput")
            input.setAttribute("oninput", "javascript: validarInfo();")

        }

    }


}


function habilitarRadios(radio) {
    // Deshabilitar todos los radio buttons excepto el que se seleccionó
    var radioButtons = document.getElementsByName("TIPO");
    for (var i = 0; i < radioButtons.length; i++) {
        if (radioButtons[i] !== radio) {
            radioButtons[i].checked = false;
        }
    }
}


function validarInput() {
    var f = document.getElementById("gtin").value;
  
    if (f == "" || f == null) {
        var h = document.getElementById("submitBtnSI");
        h.setAttribute('disable', '');
        h.style.opacity = 0.5;

    }
}

// Función para guardar los datos del formulario en el almacenamiento local
function guardarDatosFormulario() {
    var formulario = document.getElementById('frmGtin');
    var formData = new FormData(formulario);
    var datos = {};
    var resul = document.getElementById("resultado").innerHTML;
   

    

    // Recorre los campos del formulario y guarda sus valores en el objeto 'datos'
    for (var pair of formData.entries()) {
        datos[pair[0]] = pair[1];
    }
    datos['resultado'] = resul
    datos['length'] = f
    // Guarda los datos en el almacenamiento local
    localStorage.setItem('datosFormulario', JSON.stringify(datos));
}

// Función para cargar los datos guardados en el formulario
function cargarDatosFormulario() {
    var datosGuardados = localStorage.getItem('datosFormulario');
    var resultado = document.getElementById('resultado');
    var inputGtin = document.getElementById('gtin');
    if (datosGuardados) {
        var datos = JSON.parse(datosGuardados);
        var formulario = document.getElementById('frmGtin');

        for (var key in datos) {
            if (datos.hasOwnProperty(key)) {
                var input = formulario.elements[key];

                if (input) {
                    if (input.type === 'radio') {
                        if (input.value === datos[key]) {
                            input.checked = true;
                            mostrarTexto(input); // Llama a la función mostrarTexto para procesar el radio button seleccionado
                        }
                    } else {
                        input.value = datos[key];
                        //res.innerHTML = datos['resultado']
                        //mostrarTexto(input);
                    }
                }
            }
        }
        resultado.innerHTML = datos['resultado'];

        if (datos['TIPO'] == "gs1datamatrix" || datos['TIPO'] == 'qrcode' || datos['TIPO'] == 'gs1-128') {
            inputGtin.removeAttribute("maxlength")
            inputGtin.removeAttribute("oninput")
        }
        else {
            inputGtin.setAttribute("maxLength", datos['length']);
            inputGtin.setAttribute("oninput", "javascript:  if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);this.value = this.value.replace(/[^0-9]/gi, ''); validarInfo();");

        }
        


        localStorage.removeItem('datosFormulario'); // Elimina los datos del almacenamiento local
    }
}

