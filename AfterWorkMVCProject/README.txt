Scaffold-DbContext "CONNECTION STRING" Microsoft.EntityFrameworkCore.SqlServer -OutputDir "Models/Entities" -Context "MyContext" -Force -NoOnConfiguring




<div class="modal fade" id="modalDescribeWord" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalCenterTitle">Skriv definition</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Skriv en definition för det här ordet:</p>
                <h2 id="word-to-describe">@Model.Desc.TheWord</h2>
                <p style="display: none" id="description">@Model.Desc.WordDescription</p>
            </div>
            <div class="modal-footer">
                <label for="message-text" class="col-form-label"></label>
                <textarea class="form-control" id="description-text"></textarea>
                <button type="submit" class="btn btn-dark btn-modal" id="btn-description-done" data-bs-dismiss="modal">Skicka</button>
            </div>

        </div>
    </div>
</div>