// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Google Maps
 * */
var map;
function initMap() {
  // Coordinates for John Abbott.
  const myLatLng = { lat: 45.407082014717304, lng: -73.94177111534077 };

  // Initialize map
  const map = new google.maps.Map(document.getElementById("map"), {
    zoom: 12,
    center: myLatLng,
  });

  // Initialize marker on map
  new google.maps.Marker({
    position: myLatLng,
    map,
  });
}


/**
 * Navbar
 * */
document.getElementById("english-lang").addEventListener("click", () => {
  document.getElementById("current-lang").textContent = "English";
});

document.getElementById("french-lang").addEventListener("click", () => {
  document.getElementById("current-lang").textContent = "French";
})


/**
 * ReCaptcha
 * */
function enableSubmitBtn() {
  document.getElementById("submitBtn").disabled = false;
}

function resetReCaptcha() {
  grecaptcha.reset();
}


/**
 * Menu
 * */
function updatePrice(target, pricesBySize) {
  const price = target.parentElement.getElementsByClassName("item-price")[0];
  const size = target.value;

  // Set the price UI element to the price that corresponds to the selected size
  pricesBySize.forEach(ps => {
    if (ps.Size == size) {
      price.textContent = "$" + ps.Price;
      return;
    }
  });
}

