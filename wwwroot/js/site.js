// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deletePlace(placeId)
{
  $.ajax({
    url: '/Place/Delete',
    type: 'POST',
    data: {
      id: placeId
    }
  }).then(function(data) {
    window.location = '/Place';
  })
}

function flashcardImageUpdate()
{
  var imageFile = $('#flashcard-image').prop('files');
  $('#preview-flashcard-image').attr('src', imageFile.toDataUrl());
  // $('#preview-flashcard-image').attr('src', 'https://static.wikia.nocookie.net/ficspecies/images/2/2a/Dragonite.jpg');
}