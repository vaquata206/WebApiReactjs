import React from 'react';
import { Card } from 'react-bootstrap';
import LinesEllipsis from 'react-lines-ellipsis';

class AppCard extends React.Component {
    render() {
        const { app, loader } = this.props;
        if (loader) {
            return (
                <Card className="appcard">
                    <figure className="card-img-top appcard-img loading" style={{ height: "183px" }} />
                    <Card.Body>
                        <Card.Title className="loading" style={{ height: "24px" }} />
                        <Card.Text className="loading" style={{ height: "96px" }} />
                    </Card.Body>
                </Card>
                );
        }
        return (
            <Card className="appcard">
                <Card.Img className="appcard-img" variant="top" src="images/app-icon-default.png" style={{ backgroundColor: "#E2E2E2", height: "183px" }} />
                <Card.Body>
                    <Card.Title><a href={app.url} target="_blank">{app.ten_ChuongTrinh}</a></Card.Title>
                    <div className="card-text" title={app.mo_Ta}>
                        <LinesEllipsis text={app.mo_Ta} maxLine="4" component="p"/>
                    </div>
                </Card.Body>
            </Card>
            );
    }
}

export default AppCard;