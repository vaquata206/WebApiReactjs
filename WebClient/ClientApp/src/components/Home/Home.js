import React from 'react';
import { Col } from 'react-bootstrap';
import axios from 'axios';
import { ApiPaths } from "../../helpers/api";
import { alertHelper, titleHeader } from "../../helpers/utils";
import AppCard from "./AppCard";

class Home extends React.Component {
    constructor(props, context) {
        super(props, context);

        titleHeader.set("Trang chủ");

        this.state = {
            loadingApp: false,
            apps: []
        };
    }

    componentWillMount() {
        this.setState({ loadingApp: true });
        axios.get(ApiPaths.GetUserApps).then(response => {
            var list = response.data;
            this.setState({ apps: list });
        }).catch(error => {
            const message = typeof error.response.data === "string" ? error.response.data : "";
            alertHelper.show({
                variant: "danger",
                content: "Lấy danh sách chương không thành công. " + message
            });
        }).then(() => {
            this.setState({ loadingApp: false });
        });
    }

    render() {
        const { loadingApp, apps } = this.state;
        return (
            <section className="content">
                <div className="row">
                    <div className="col-lg-12">
                        <div className="box box-primary">
                            <div className="box-header with-border">
                                <h3 className="box-title">Danh sách chương trình</h3>
                            </div>
                            <div className="box-body row">
                                {loadingApp ?
                                    <Col sm={3}>
                                        <AppCard loader />
                                    </Col> :
                                    (apps || []).length > 0 ?
                                        apps.map(app => (
                                            <Col key={app.id_ChuongTrinh} sm={3}>
                                                <AppCard app={app} />
                                            </Col>)) :
                                        <Col>
                                            <div style={{ textAlign: "center" }}>
                                                <span>Không có chương trình</span>
                                            </div>
                                        </Col>
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        );
    }
}

export default Home;
